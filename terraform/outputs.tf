output "api_public_ip" {
  description = "The public IP address of the api server"
  value = aws_instance.dj_api[0].public_ip
}

output "api_public_dns" {
  description = "The public DNS address of the api server"
  value = aws_instance.dj_api[0].public_dns
}

output "database_endpoint" {
  description = "The endpoint of the database"
  value = aws_db_instance.dj_db.endpoint
}

output "database_port" {
  description = "The port of the database"
  value = aws_db_instance.dj_db.port
}