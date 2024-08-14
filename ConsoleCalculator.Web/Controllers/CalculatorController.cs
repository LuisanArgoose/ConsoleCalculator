﻿using ConsoleCalculator.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConsoleCalculator.Web.Controllers
{
    
    public class CalculatorController : Controller
    {
        private readonly ICalculator _calculator;

        public CalculatorController(ICalculator calculator)
        {
            _calculator = calculator;
        }

        [HttpGet("calculate")]
        public IActionResult Calculate([FromQuery] string expression)
        {
            try
            {
                var result = _calculator.Calculate(expression);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Invalid expression: " + ex.Message);
            }
        }
    }
}