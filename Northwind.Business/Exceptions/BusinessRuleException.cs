﻿namespace Northwind.Business.Exceptions;

public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message)
    {
        
    }
}
