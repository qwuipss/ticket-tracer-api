namespace TicketTracer.Data.Entities;

public static class EntityConstraints
{
    public static class Project
    {
        public const int TitleMaxLength = 64;
    }

    public static class Board
    {
        public const int TitleMaxLength = 64;
    }

    public static class Ticket
    {
        public const int TitleMaxLength = 64;

        public const int DescriptionMaxLength = 8192;
    }

    public static class User
    {
        public const int EmailMaxLength = 64;

        public const int NameMaxLength = 32;

        public const int SurnameMaxLength = 32;
    }

    public static class Attribute
    {
        public const int NameMaxLength = 32;
    }

    public static class AttributeValue
    {
        public const int ValueMaxLength = 32;
    }
}