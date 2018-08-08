
namespace FSharpSound

open System


type NoteName =
    | c = 0
    | csh = 1
    | d = 2
    | dsh = 3
    | e = 4
    | f = 5
    | fsh = 6
    | g = 7
    | gsh = 8
    | a = 9
    | ash = 10
    | b = 11

type Note =
    struct
        static member basePitch = 440.0
 
        val noteName: NoteName
        val octave: int
        new(noteName: NoteName, octave: int) = { noteName = noteName; octave = octave }

        member this.noteIndex  = this.octave * 12 + (int)this.noteName
        member this.pitch =
            Note.basePitch * Math.Pow(2.0, float (this.noteIndex - Note(NoteName.ash, 4).noteIndex) / 12.0)
        member this.perCycle (sec: float) =
            let x = sec * this.pitch
            x - floor(x)

    end


