﻿using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;
using System;
using System.Text;
using System.Text.Json;

namespace PlatformService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["rabbitMQHost"],
                Port = int.Parse(_configuration["rabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
                Console.WriteLine("--> connected to MessageBus");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> could not connect to Message Bus: {ex.Message}");
            }
        }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }

        public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
        {
            var message = JsonSerializer.Serialize(platformPublishedDto);
            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection is open, sending message");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ Connection is closed, Not sending message");
            }
        }
        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange:"trigger",routingKey:"",basicProperties:null,body:body);
            Console.WriteLine($"--> Sending Message: {message}");
        }
        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }
    }
}
