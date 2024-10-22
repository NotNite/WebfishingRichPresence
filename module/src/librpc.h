#ifndef LIBRPC_H
#define LIBRPC_H

#include <Godot.hpp>
#include <xstring>
#include <discord/cpp/core.h>

using namespace godot;

class LibRPC : public Object {
  GODOT_CLASS(LibRPC, Object)

public:
  static void _register_methods();

  LibRPC();
  ~LibRPC();

  void _init();
  void run_callbacks();
  void set_status(String str);
  void set_num_players(int num);

private:
  void update_status();

  discord::Core* core;
  discord::Timestamp started_at;
  godot::String status;
  int num_players;
};

#endif
