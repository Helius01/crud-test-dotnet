using System.Net.Mail;

namespace Customers.Domain.ValueObjects;

public sealed record Email
{
    public string Value { get; }
    private Email(string value) => Value = value;

    public static Email Create(string rawValue)
    {
        rawValue ??= "";
        var trimmed = rawValue.Trim();
        if (string.IsNullOrWhiteSpace(trimmed))
            throw new ArgumentException("Email is required.", nameof(rawValue));

        try
        {
            var addr = new MailAddress(trimmed);
            if (addr.Address != trimmed)
                throw new ArgumentException("Invalid email.");
        }
        catch
        {
            throw new ArgumentException("Invalid email.", nameof(rawValue));
        }

        return new Email(trimmed.ToLowerInvariant());
    }
    public override string ToString() => Value;
}
