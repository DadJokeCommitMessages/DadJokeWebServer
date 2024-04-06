terraform {
  required_version = ">= 0.13"
  required_providers {
    aws = {
      source  = "hashicorp/aws"
      version = "~> 3.0"
    }
  }
}

provider "aws" {
  region = "eu-west-1"
}

resource "aws_key_pair" "dj_kp" {
  key_name = "dj_kp"
  public_key = var.public_key

  tags = {
    name = "dj_kp"
    owner = "thashil.naidoo@bbd.co.za"
    created-using = "terraform"
  }
}

data "template_file" "user_data" {
  template = file("install-dotnet.sh")
}

resource "aws_instance" "dj_api" {
  ami = var.ec2_ami
  count = var.ec2_count
  instance_type = var.ec2_instance_type
  key_name = aws_key_pair.dj_kp.key_name
  security_groups = [element(var.ec2_sg, count.index)]
  subnet_id = var.ec2_subnet_id

  # provisioner "file" {
  #   source      = "install-dotnet.sh"  # Path to your script file
  #   destination = "/tmp/install-dotnet.sh"     # Destination path on the EC2 instance

  #   connection {
  #     type        = "ssh"
  #     user        = "ubuntu"
  #     private_key = file("dj_kp")  # Path to your private key
  #     host        = self.public_ip
  #   }
  # }

  # provisioner "remote-exec" {
  #   inline = [
  #     "chmod +x /tmp/install-dotnet.sh",       # Make the script executable
  #     "/tmp/install-dotnet.sh"                 # Execute the script
  #   ]

  #   connection {
  #     type        = "ssh"
  #     user        = "ubuntu"
  #     private_key = file("dj_kp")  # Path to your private key
  #     host        = self.public_ip
  #   }
  # }

  user_data = data.template_file.user_data.rendered

  tags = {
    Name = "dj_api"
    owner = "thashil.naidoo@bbd.co.za"
    created-using = "terraform"
  }
}

resource "aws_db_instance" "dj_db" {
  allocated_storage      = var.ec2_db_storage
  storage_type           = var.ec2_db_storage_type
  identifier             = var.ec2_db_identifier
  engine                 = var.ec2_db_engine
  engine_version         = var.ec2_db_version
  instance_class         = var.ec2_db_instance_class
  username               = var.ec2_db_user
  password               = var.ec2_db_password
  publicly_accessible    = true
  skip_final_snapshot    = true
  vpc_security_group_ids = ["${var.ec2_db_vpc_security_group_id}"]

  tags = {
    Name = "dj_db"
    owner = "thashil.naidoo@bbd.co.za"
    created-using = "terraform"
  }
}