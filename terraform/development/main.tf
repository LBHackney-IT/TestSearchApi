provider "aws" {
  region  = "eu-west-2"
  version = "~> 2.0"
}
data "aws_caller_identity" "current" {}
data "aws_region" "current" {}
locals {
  parameter_store = "arn:aws:ssm:${data.aws_region.current.name}:${data.aws_caller_identity.current.account_id}:parameter"
}

terraform {
  backend "s3" {
    bucket  = "terraform-state-housing-development"
    encrypt = true
    region  = "eu-west-2"
    key     = "services/test-search-api/state"
  }
}

/*    ELASTICSEARCH SETUP    */

data "aws_vpc" "development_vpc" {
  tags = {
    Name = "test_search_api_dev"
  }
}

data "aws_subnet_ids" "development" {
  vpc_id = data.aws_vpc.development_vpc.id
  filter {
    name   = "tag:Type"
    values = ["private"]
  }
}

module "elasticsearch_db_development" {
  source           = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/database/elasticsearch"
  vpc_id           = data.aws_vpc.development_vpc.id
  environment_name = "development"
  port             = 443
  domain_name      = "test_search_api_es"
  subnet_ids       = [tolist(data.aws_subnet_ids.development.ids)[0]]
  project_name     = "test_search_api"
  es_version       = "7.8"
  encrypt_at_rest  = "true"
  instance_type    = "t3.small.elasticsearch"
  instance_count   = "2"
  ebs_enabled      = "true"
  ebs_volume_size  = "10"
  region           = data.aws_region.current.name
  account_id       = data.aws_caller_identity.current.account_id
}

resource "aws_ssm_parameter" "search_elasticsearch_domain" {
  name  = "/test-search-api/development/elasticsearch-domain"
  type  = "String"
  value = "https://${module.elasticsearch_db_development.es_endpoint_url}"
}

module "housing_search_api_cloudwatch_dashboard" {
  source                  = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/cloudwatch/dashboards/api-dashboard"
  environment_name        = var.environment_name
  api_name                = "test_search_api"
  include_sns_widget      = false
  include_dynamodb_widget = false
  no_sns_widget_dashboard = false
}

data "aws_ssm_parameter" "cloudwatch_topic_arn" {
  name = "/housing-tl/${var.environment_name}/cloudwatch-alarms-topic-arn"
}

module "api-alarm" {
  source           = "github.com/LBHackney-IT/aws-hackney-common-terraform.git//modules/cloudwatch/api-alarm"
  environment_name = var.environment_name
  api_name         = "test_search_api"
  alarm_period     = "300"
  error_threshold  = "1"
  sns_topic_arn    = data.aws_ssm_parameter.cloudwatch_topic_arn.value
}
