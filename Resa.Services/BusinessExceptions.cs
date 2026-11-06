namespace Resa.Services;

/// <summary>Donnée invalide (dates inversées, capacité dépassée, salle/user inexistant...) -> HTTP 400.</summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}

/// <summary>Conflit avec l'état actuel (créneau déjà pris...) -> HTTP 409.</summary>
public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}
