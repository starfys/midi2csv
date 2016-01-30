import sys
#print ("1\n2 a4 c8\n4 d4 f8\n8 a4 c8")
#print (sys.argv[1])
with open(sys.argv[1], "w") as f:
	f.write("2 20")
	f.write("1000 10 1000")
	f.write("2000 10 1000")
	f.write("3000 60 1000")
	