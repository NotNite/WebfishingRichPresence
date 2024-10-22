#include "librpc.h"
#include <chrono>
#include <utility>

void LibRPC::_register_methods() {
    register_method("run_callbacks", &LibRPC::run_callbacks);
    register_method("set_status", &LibRPC::set_status);
    register_method("set_num_players", &LibRPC::set_num_players);
}

LibRPC::LibRPC() {
    discord::Core::Create(1298354101498941521, 0, &this->core);
    this->started_at = std::chrono::duration_cast<std::chrono::milliseconds>(
        std::chrono::system_clock::now().time_since_epoch()).count();
}

LibRPC::~LibRPC() {
}

void LibRPC::run_callbacks() {
    this->core->RunCallbacks();
}

void LibRPC::update_status() {
    auto activity = discord::Activity();
    activity.SetName("WEBFISHING");
    activity.SetDetails(this->status.alloc_c_string());
    activity.GetTimestamps().SetStart(this->started_at);
    if (this->num_players != -1) {
        //Godot::print("Setting party size");
        activity.GetParty().GetSize().SetCurrentSize(this->num_players);
        activity.GetParty().GetSize().SetMaxSize(12);
    }
    this->core->ActivityManager().UpdateActivity(activity, [](discord::Result result) {
        //Godot::print("Activity updated", static_cast<int>(result));
    });
}


void LibRPC::set_status(String str) {
    this->status = std::move(str);
    this->update_status();
}

void LibRPC::set_num_players(int num) {
    this->num_players = num;
    this->update_status();
}

void LibRPC::_init() {
}
