open System
open FSharpSound

let sampleRate = 48000

let samplesToSec rate sample = (float)sample / (float)rate

let sinWave t = sin(2.0 * Math.PI * t)
let squareWave t = 1.0 - round(float t) * 2.0

let volume rate value = rate * value

let fn noteName octave sec = sec |> Note(noteName, octave).perCycle |> squareWave |> volume 0.3

[<EntryPoint>]
let main argv = 
    [0..sampleRate * 2]
    |> List.map(fun x ->
        x
        |> samplesToSec sampleRate
        |> (fun x -> (x |> (fn NoteName.c 4)) + (x |> (fn NoteName.e 4)) + (x |> (fn NoteName.g 4)))
        |> float32)
    |> SoundIOOutStreamUtil.playsound sampleRate
    0 // return an integer exit code
