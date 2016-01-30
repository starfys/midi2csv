#!/usr/bin/python2
import midi
import sys

def compare(event1, event2):
 
    if event1[0] < event2[0]:
        return -1
    elif event1[0] > event2[0]:
        return 1
    else:
        if event1[2] == 0:
            return 1
        if event2[2] == 0:
            return -1   
        return 0

tempo = 500000.0
pattern = midi.read_midifile(sys.argv[1])
output_file = open(sys.argv[2], 'w+')
output_file_raw = open(sys.argv[2]+".raw", 'w+')
print >>output_file_raw, pattern
events = []
for track in pattern:
    track.make_ticks_abs()
    for event in track:
        events.append(event)

#Used to keep track of note positions
turned_on = {}
#Used to keep track of played notes
notes = []
for event in events:
    if type(event) == midi.NoteOnEvent:
        turned_on[event.data[0]] = event.tick
        if event.data[0] == 0:
            notes.append((turned_on[event.data[0]], event.data[0], event.tick - turned_on[event.data[0]]))
    elif type(event) == midi.NoteOffEvent:
        notes.append((turned_on[event.data[0]], event.data[0], event.tick - turned_on[event.data[0]]))
    elif type(event) == midi.SetTempoEvent:
        notes.append((event.tick, (((event.data[0]*65536)+event.data[1]*256+event.data[2])/1000000.0), 0))

notes = sorted(notes,cmp=compare)

print >>output_file, len(notes)
for note in notes:
    if note[2] == 0:
        print >>output_file, note[0], "%.2f" % round(note[1],2), note[2]
    else:
        print >>output_file, note[0], note[1], note[2]
