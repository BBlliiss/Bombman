using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatInt
{
    private int defaultValue;
    public Action OnValueChanged;
    public Action OnValueIncreased;
    public Action OnValueDecreased;
    public Action OnValueNegative;

    public StatInt(int _defaultValue,Action _onValueChanged = null, Action _onValueIncreased = null, Action _onValueDecreased = null,
        Action _onValueNegative = null)
    {
        defaultValue = _defaultValue;
        OnValueChanged = _onValueChanged;
        OnValueIncreased = _onValueIncreased;
        OnValueDecreased = _onValueDecreased;
        OnValueNegative = _onValueNegative;
        SetDefaultValue();
    }

    [SerializeField] private int value;

    public int Value
    {
        get => value;
        set
        {
            if (value < 0)
            {
                this.value = 0;
                OnValueNegative?.Invoke();
            }
            else
            {
                this.value = value;
            }
            
            OnValueChanged?.Invoke();
        }
    }

    public void SetDefaultValue()
    {
        Value = defaultValue;
    }

    public void SetValue(int _value)
    {
        if (_value > Value)
        {
            Value = _value;
            OnValueIncreased?.Invoke();
        }
        else if (_value < Value)
        {
            Value = _value;
            OnValueDecreased?.Invoke();
        }
    }

    public void ChangeDefaultValue(int _value)
    {
        defaultValue = _value;
    }

    public static int operator +(StatInt a, float b)
    {
        return Mathf.RoundToInt(a.Value + b);
    }

    public static int operator -(StatInt a, float b)
    {
        return a + -b;
    }

    public static int operator *(StatInt a, float b)
    {
        return Mathf.RoundToInt(a.Value * b);
    }
}