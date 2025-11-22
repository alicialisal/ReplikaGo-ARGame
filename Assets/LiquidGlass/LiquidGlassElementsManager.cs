using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LiquidGlassElementsManager : IDisposable
{
    private const int MaxElements = 128;
    private readonly List<LiquidGlassElement> _elements = new List<LiquidGlassElement>(MaxElements);
    private bool _dirty = false;
    private ComputeBuffer _structuredBuffer;
    private bool _updateCallRegistered = false;

    private static LiquidGlassElementsManager instance;

    public static LiquidGlassElementsManager GetInstance()
    {
        if (instance == null) instance = new LiquidGlassElementsManager();
        return instance;
    }

    private LiquidGlassElementsManager()
    {
        int stride = Marshal.SizeOf(typeof(LiquidGlassElement));
        _structuredBuffer = new ComputeBuffer(MaxElements, stride, ComputeBufferType.Structured);
    }

    public void SubmitElement(ref int elementIndex, LiquidGlassElement element)
    {
        if (elementIndex < 0)
        {
            if (_elements.Count >= MaxElements) return;                // safety
            elementIndex = _elements.Count;
            _elements.Add(element);
            _dirty = true;
        }
        else if (elementIndex < _elements.Count && !_elements[elementIndex].Equals(element))
        {
            _elements[elementIndex] = element;
            _dirty = true;
        }

        if (_dirty) RegisterUpdateCall();
    }

    public void RemoveElement(int index)
    {
        if (index >= 0 && index < _elements.Count)
        {
            _elements[index] = default;
            _dirty = true;
            RegisterUpdateCall();
        }
    }

    private void RegisterUpdateCall()
    {
        if (_updateCallRegistered) return;
        RenderPipelineManager.beginCameraRendering += OnBeginCameraRendering;
        _updateCallRegistered = true;
    }

    private void DeregisterUpdateCall()
    {
        if (!_updateCallRegistered) return;
        RenderPipelineManager.beginCameraRendering -= OnBeginCameraRendering;
        _updateCallRegistered = false;
    }

    private void OnBeginCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        UploadToGPU();
        if (!_dirty) DeregisterUpdateCall();
    }

    private void UploadToGPU()
    {
        if (!_dirty) return;
        int count = _elements.Count;
        if (count == 0) return;

        // Ensure buffer size
        if (_structuredBuffer.count < count)
        {
            _structuredBuffer.Release();
            int stride = Marshal.SizeOf(typeof(LiquidGlassElement));
            _structuredBuffer = new ComputeBuffer(MaxElements, stride, ComputeBufferType.Structured);
        }

        _structuredBuffer.SetData(_elements, 0, 0, count);
        Shader.SetGlobalBuffer("_LiquidGlassElements", _structuredBuffer);
        _dirty = false;
    }

    public void Dispose()
    {
        Release();
        instance = null;
    }

    private void Release()
    {
        Shader.SetGlobalBuffer("_LiquidGlassElements", (ComputeBuffer)null);
        DeregisterUpdateCall();
        _structuredBuffer?.Release();
        _structuredBuffer = null;
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct LiquidGlassElement
{
    public Vector4 rect;
    public float radius;
    public Vector4 tint;
    public float blur;
}
