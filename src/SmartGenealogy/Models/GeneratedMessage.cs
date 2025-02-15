using System;

namespace SmartGenealogy.Models;

public class GeneratedMessage : Message
{
    private double _generationSpeed;

    public double GenerationSpeed
    {
        get => _generationSpeed;
        set => _generationSpeed = Math.Round(value, 2);
    }

    public GeneratedMessage(string content, double generationSpeed)
        : base(content)
    {
        GenerationSpeed = generationSpeed;
    }
}