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


pattern = midi.read_midifile(sys.argv[1])
tempo = 500000.0
pat_form = pattern.format
pat_form += 1
print "Format: ", pat_form
if (pat_form == 2):
    print "Format 2 not permitted! Use a different .midi, sorry."
    exit()
resolution = pattern.resolution * 1.0
output_file = open(sys.argv[2], 'w+')
#output_file_raw = open(sys.argv[2]+".raw", 'w+')
#print >>output_file_raw, pattern
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
        notes.append((event.tick, (((event.data[0]*65536)+event.data[1]*256+event.data[2])), 0))

notes = sorted(notes,cmp=compare)
final_notes = []
last_time = 0.0
last_ticks = 0
for note in notes:
    if note[2] == 0:
        tempo = note[1]
        continue
    next_time = last_time + ((tempo * (note[0] - last_ticks)) / (resolution * 1000000))
    final_notes.append((next_time, note[1], (tempo * note[2]) / (resolution * 1000000)))
    last_ticks = note[0]
    last_time = next_time
print >>output_file, len(final_notes)
for note in final_notes:
        print >>output_file, "%.2f" % round(note[0],2), note[1], "%.2f" % round(note[2], 2)
