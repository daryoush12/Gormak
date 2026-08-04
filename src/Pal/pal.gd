class_name Pal

const MAX_HAPPINESS = 100
const MAX_HUNGER = 100
const MAX_HEALTH = 100

var health = MAX_HEALTH
var hunger = MAX_HUNGER;
var happiness = MAX_HAPPINESS;

func _init(health, hunger, happiness):
	self.health = health || MAX_HEALTH
	self.happiness = happiness || MAX_HAPPINESS
	self.hunger = hunger || MAX_HUNGER
	
	print("Initiated pal {0},{1},{2}".format([health, happiness, hunger]))
	
func feed():
	hunger = clamp(hunger + 40, 0, MAX_HAPPINESS)
	notify()

func pet():
	happiness = clamp(happiness + 40, 0, MAX_HAPPINESS)
	notify()
	
func notify():
	emit_signal("pal_state_change", [hunger, health, happiness])
