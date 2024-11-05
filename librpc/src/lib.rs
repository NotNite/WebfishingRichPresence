use discord_rich_presence::{
    activity::{Activity, Assets, Party, Timestamps},
    DiscordIpc, DiscordIpcClient,
};
use gdnative::prelude::*;
use std::time::SystemTime;

#[derive(NativeClass)]
#[inherit(Node)]
pub struct LibRPC {
    client: DiscordIpcClient,
    status: String,
    num_players: Option<i32>,
    started_at: i64,
}

#[methods]
impl LibRPC {
    fn new(_base: &Node) -> Self {
        let mut client = DiscordIpcClient::new("1298354101498941521").unwrap();
        client.connect().ok();
        LibRPC {
            client,
            status: String::new(),
            num_players: None,
            started_at: std::time::SystemTime::now()
                .duration_since(SystemTime::UNIX_EPOCH)
                .unwrap()
                .as_secs() as i64,
        }
    }

    #[method]
    fn update_status(&mut self) {
        let mut activity = Activity::new()
            .details(self.status.as_ref())
            .timestamps(Timestamps::new().start(self.started_at))
            .assets(Assets::new().large_image("webfishing"));

        if let Some(num) = self.num_players {
            let party = Party::new().size([num, 12]);
            activity = activity.party(party);
        }

        self.client.set_activity(activity).ok();
    }

    #[method]
    fn set_status(&mut self, status: GodotString) {
        self.status = status.to_string();
        self.update_status();
    }

    #[method]
    fn set_num_players(&mut self, num: i64) {
        if num == -1 {
            self.num_players = None;
        } else {
            self.num_players = Some(num as i32);
        }
        self.update_status();
    }
}

fn init(handle: InitHandle) {
    handle.add_class::<LibRPC>();
}

godot_init!(init);
