namespace ServiceLayer.Exceptions;

public class DuplicateDomainNameException(string duplicateName)
    : ApplicationException($"Found more than one Domain with name: {duplicateName}")
{}