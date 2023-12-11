namespace Answer.King.IntegrationTests.POCO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class ErrorResponse
{
    public required object Errors { get; set; }
    public required int Status {  get; set; }   
}
