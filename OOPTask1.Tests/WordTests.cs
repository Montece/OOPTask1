﻿using OOPTask1.Model;
using Xunit;

namespace OOPTask1.Tests;

public sealed class WordTests
{
    [Fact]
    public void Create_Empty()
    {
        Assert.Throws<ArgumentException>(() => new Word(string.Empty));
    }

    [Fact]
    public void Create_Null()
    {
        Assert.Throws<ArgumentNullException>(() => new Word(null));
    }

    [Theory]
    [InlineData(["Привет"])]
    [InlineData(["Hi"])]
    public void Create_GoodValue(string wordStr)
    {
        var word = new Word(wordStr);

        Assert.Equal(word.Value, wordStr);
    }

    [Theory]
    [InlineData(["Привет!"])]
    [InlineData(["Hi!"])]
    public void Create_BadValue(string wordStr)
    {
        Assert.Throws<ArgumentException>(() => new Word(wordStr));
    }

    [Fact]
    public void Equals_WordAndString()
    {
        var wordStr = "test";
        var word = new Word(wordStr);

        var wordEqualsString = word.CustomEquals(wordStr);
        var wordEqualsWord = word.CustomEquals(word);

        Assert.True(wordEqualsString && wordEqualsWord);
    }
}