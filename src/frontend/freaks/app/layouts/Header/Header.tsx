import Link from "next/link";
import styles from "./_styles.module.scss";
import Profile from "./components/Profile/Profile";
import {auth} from "@/api/auth/auth";
import CustomImage from "@/components/ui/CustomImage";

const Header = async () => {
  const session = await auth();
  const { idToken } = session ?? {};
  const isAuthenticated = !!session;

  const userRoles = (session?.user?.roles || []) as string[];
  const isAdmin = userRoles.includes("admin");

  return (
    <header className={styles.header}>
      <div className={styles.wrapper}>
        <div className={styles.imageWrapper}>
          <CustomImage src={"/images/logo.png"} alt="logo" fill />
        </div>
        <div className={styles.actionButtons}>
          {isAdmin && (
            <Link href="/admin-panel" className={styles.adminPanelLink}>
              Панель администратора
            </Link>
          )}
          <Profile
            session={session}
            isAuthenticated={isAuthenticated}
            idToken={idToken}
          />
        </div>
      </div>
    </header>
  );
};

export default Header;
