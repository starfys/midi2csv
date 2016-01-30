#!/usr/bin/python2
import midi
import sys
tempo = 2000.0
pattern = midi.read_midifile(sys.argv[1])
output_file = open(sys.argv[2], 'w+')
#tempoTrack = pattern[0]
#tempoTrack.make_ticks_abs()
#print(tempoTrack)
#Get the main track
pattern = pattern[1]
#Fix ticks
pattern.make_ticks_abs()
#Used to keep track of note positions
turned_on = {}
#Used to keep track of played notes
notes = []
for event in pattern:
    if type(event) == midi.NoteOnEvent:
        turned_on[event.data[0]] = event.tick
        if event.data[0] == 0:
            notes.append((turned_on[event.data[0]], event.data[0], event.tick - turned_on[event.data[0]]))
    elif type(event) == midi.NoteOffEvent:
        notes.append((turned_on[event.data[0]], event.data[0], event.tick - turned_on[event.data[0]]))
notes.sort()
print >>output_file, len(notes)
for note in notes:
    print >>output_file, note[0]/tempo, note[1], note[2]/tempo
