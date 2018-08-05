module FSharpSound.SoundIOOutStreamUtil

#nowarn "9"

open SoundIOSharp
open FSharp.NativeInterop
open System.Threading

let playsound sampleRate (stream: System.Collections.Generic.IEnumerable<float32>) =
    use api = new SoundIO()
    api.Connect()
    api.FlushEvents()
    let device = api.GetOutputDevice(api.DefaultOutputDeviceIndex)
    try
        use outstream = device.CreateOutStream()
        use waiter = new ManualResetEventSlim()
        use en = stream.GetEnumerator()

        outstream.WriteCallback <- fun frameCountMin frameCountMax ->
            let mutable frameCount = 0
            frameCount <- frameCountMax
            let layout2 = outstream.Layout
            let results = outstream.BeginWrite(ref frameCount)
            try
                let rec iterate () =
                    if en.MoveNext() then
                        let value = en.Current
                        for c in 0..layout2.ChannelCount - 1 do
                            let mutable area = results.GetArea(c)
                            NativePtr.set (area.Pointer |> NativePtr.ofNativeInt<float32>) 0 value
                            area.Pointer <- NativeInterop.NativePtr.add (area.Pointer |> NativePtr.ofNativeInt<byte>) area.Step |> NativePtr.toNativeInt
                        iterate()
                iterate()
            finally
                outstream.EndWrite()
                waiter.Set()
        outstream.Format <- SoundIODevice.Float32NE
        outstream.SampleRate <- sampleRate
        outstream.SoftwareLatency <- 0.0

        outstream.Open()
        outstream.Start()

        waiter.Wait()
    finally
        device.RemoveReference()
