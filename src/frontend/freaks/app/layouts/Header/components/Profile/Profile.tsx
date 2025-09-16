"use client";

import React from "react";

import Avatar from "@mui/material/Avatar";
import styles from "./_styles.module.scss";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import { IconButton } from "@mui/material";
import { Session } from "next-auth";
import { useAuthActions } from "@/domains/auth/hooks/ useAuthActions";

type Props = {
  session: Session | null;
  idToken?: string;
  isAuthenticated: boolean;
};

// function stringToColor(s: string) {
//   let hash = 0;
//   for (let i = 0; i < s.length; i++)
//     hash = s.charCodeAt(i) + ((hash << 5) - hash);
//   return `#${((hash >>> 0) & 0xffffff).toString(16).padStart(6, "0")}`;
// }

export function stringAvatar(name: string) {
  const n = (name || "U").trim();
  const parts = n.split(/\s+/);
  const initials =
    (parts[0]?.[0] ?? "U") + (parts[1]?.[0] ?? parts[0]?.[1] ?? "");
  return {
    // sx: { bgcolor: stringToColor(n) },
    children: initials.toUpperCase(),
  };
}

const Profile = ({ session, isAuthenticated, idToken }: Props) => {
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);

  const { handleLogout } = useAuthActions({ isAuthenticated, idToken });

  if (!session) return <h2>НЕту</h2>;

  const open = Boolean(anchorEl);
  const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <div className={styles.profile}>
      <IconButton
        id="basic-button"
        aria-controls={open ? "basic-menu" : undefined}
        aria-haspopup="true"
        aria-expanded={open ? "true" : undefined}
        onClick={handleClick}
      >
        <Avatar
          alt="User avatar"
          {...stringAvatar(session?.user.game_nickname || "")}
        />
      </IconButton>

      <Menu
        id="basic-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        slotProps={{
          list: {
            "aria-labelledby": "basic-button",
          },
        }}
      >
        <MenuItem onClick={handleClose}>Profile</MenuItem>
        <MenuItem onClick={handleClose}>My account</MenuItem>
        <MenuItem onClick={handleLogout}>Выход</MenuItem>
      </Menu>
    </div>
  );
};

export default Profile;
