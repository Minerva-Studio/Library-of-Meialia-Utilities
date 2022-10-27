﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Minerva.Module
{
    [Serializable]
    public class Argument : IEquatable<Argument>, IComparable<Argument>
    {
        [SerializeField] private string key;
        [SerializeField] private string value;
        public Argument(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Value { get => value; set => this.value = value; }
        public string Key { get => key; set => key = value; }


        public bool Equals(Argument other)
        {
            return other.value == value && other.key == key;
        }

        public override bool Equals(object obj)
        {
            return obj is Argument ? Equals((Argument)obj) : false;
        }

        public int CompareTo(Argument other)
        {
            return key.CompareTo(other.key);
        }

        public override int GetHashCode()
        {
            return key.GetHashCode();
        }
    }

    public static class ArgumentExtensions
    {
        public static bool ContainsKey(this IEnumerable<Argument> nameables, string key)
        {
            foreach (var nameable in nameables)
            {
                if (nameable.Key == key) return true;
            }
            return false;
        }

        public static string GetValue(this IEnumerable<Argument> nameables, string key)
        {
            foreach (var nameable in nameables)
            {
                if (nameable.Key == key) return nameable.Value;
            }
            return string.Empty;
        }

        public static bool SetValue(this IEnumerable<Argument> nameables, string key, string value)
        {
            foreach (var nameable in nameables)
            {
                if (nameable.Key == key)
                {
                    nameable.Value = value;
                }
                return true;
            }
            return false;
        }
    }
}