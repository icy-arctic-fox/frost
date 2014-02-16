    def console_command load_map (map_name:string) {
		print(map_name);
	}
	
	def function say_hello (name) {
		print("Hello, " + name + "!");
	}
	say_hello("Michael");
	
	// Sample of a bullet pattern definition
	def bullet_pattern rising_tide (t:int) {
		x = cos(t);
		y = sin(t);
	}
	
	def class ClassName {
		new () {
			// Constructor goes here
		}
		def function do_something () {
			print("I'm doing something...");
		}
	}
	
	var obj = new ClassName;
	obj.do_something();
	
	// Force a variable to only accept one type
	var strict_obj:ClassName = new ClassName;