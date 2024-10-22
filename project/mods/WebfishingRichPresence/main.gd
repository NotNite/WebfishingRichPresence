extends Node

var rpc = null
var node = null

func _ready():
	var rpc = load("res://mods/WebfishingRichPresence/librpc.gdns").instance()
	node = Node.new()
	self.add_child(node)
	node.set_script(rpc)

func _process(delta):
	if node: node.run_callbacks()

func set_status(s):
	if not node: return
	if s == "#menu":
		node.set_status("sitting at the main menu")
		set_num_players(-1)
	elif s == "#default":
		node.set_status("fishing at pawprint point")
	elif s == "#rain":
		node.set_status("sloshing about in the rain")
	else:
		print("Unknown status: ", s)

func set_num_players(num):
	if not node: return
	node.set_num_players(num)
