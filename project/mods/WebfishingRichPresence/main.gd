extends Node

var rpc = null
var node = null

func _ready():
	var lib = GDNativeLibrary.new()
	var cfg = ConfigFile.new()
	cfg.set_value("entry", "Windows.64", "%LIBRPCPATH%")
	lib.config_file = cfg

	var script = NativeScript.new()
	script.library = lib
	script.resource_name = "librpc"
	script.set_class_name("LibRPC")

	node = Node.new()
	self.add_child(node)
	node.set_script(script)

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
