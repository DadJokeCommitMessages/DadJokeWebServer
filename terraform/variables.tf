variable "ec2_ami" {
  default = "ami-0c1c30571d2dae5c9"
}

variable "ec2_instance_type" {
  default = "t2.micro"
}

variable "ec2_count" {
  type    = number
  default = 1
}

variable "ec2_sg" {
  default = ["sg-008fecdc0471fe360"] //
}

variable "public_key" {
  description = "Public key for SSH"
  type = string
  sensitive = true
}

variable "ec2_subnet_id" {
  default = "subnet-09aea05f4ab73bc68" //
}

variable "ec2_vpc_id" {
  default = "vpc-0c7fcc842735d06aa" //
}

variable "ec2_db_engine" {
  default = "postgres"
}

variable "ec2_db_instance_class" {
  default = "db.t3.micro"
}

variable "ec2_db_version" {
  default = "16.1"
}

variable "ec2_db_user" {
  default = "postgres"
}

variable "ec2_db_password" {
  description = "Database master user password"
  type = string
  sensitive = true
}

variable "ec2_db_identifier" {
  default = "dj-database"
}

variable "ec2_db_storage" {
  type    = number
  default = "20"
}

variable "ec2_db_storage_type" {
  default = "gp2"
}

variable "ec2_db_vpc_security_group_id" {
  default = "sg-008fecdc0471fe360" //
}