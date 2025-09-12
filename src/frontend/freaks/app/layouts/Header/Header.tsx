import { CustomContainer } from "@/components/ui/CustomContainer";
import styles from "./_styles.module.scss";
import Profile from "./components/Profile/Profile";
import { auth } from "@/api/auth/auth";
import Image from "next/image";
import CustomImage from "@/components/ui/CustomImage";

const Header = async () => {
  const session = await auth();
  const { idToken } = session ?? {};
  const isAuthenticated = !!session;

  return (
    <header className={styles.header}>
      <CustomContainer maxWidth="lg">
        <div className={styles.wrapper}>
          <div className={styles.imageWrapper}>
            <CustomImage src={"/images/logo.png"} alt="logo" fill />
          </div>
          <Profile
            session={session}
            isAuthenticated={isAuthenticated}
            idToken={idToken}
          />
        </div>
      </CustomContainer>
    </header>
  );
};

export default Header;
