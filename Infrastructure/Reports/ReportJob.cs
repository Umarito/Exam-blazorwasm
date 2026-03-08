// using System;
// using Quartz;
// using WebApi.EmailService;

// namespace WebApi.Workers;

// public class ReportJob(IEmailService emailService) : IJob
// {
//     private readonly IEmailService _emailService = emailService;
//     public async Task Execute(IJobExecutionContext context)
//     {
//         try
//         {
            
//         Console.WriteLine("Sending report...");
//         await _emailService.SendAsync("u9884118@gmail.com", "testing background jobs", "this is testing process, do not response to message.");
//         await Task.CompletedTask;
//         }
//         catch(Exception ex)
//         {
//             System.Console.WriteLine("VAAAAAAAAAA KAFIIIIIIIIIIDDDDD"+ex.Message);
//         }
//     }
// }