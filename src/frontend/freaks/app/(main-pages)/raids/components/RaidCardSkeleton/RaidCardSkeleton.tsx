import { Skeleton } from "@mui/material";
import styles from "./_styles.module.scss";

const RaidCardSkeleton = () => {
  return (
    <div className={styles.raid}>
      <div className={styles.left}>
        <Skeleton
          variant="rectangular"
          width={60}
          height={24}
          sx={{
            borderRadius: "12px",
            background: "rgba(255, 255, 255, 0.1)",
          }}
        />

        <Skeleton
          variant="rectangular"
          width={60}
          height={24}
          sx={{
            borderRadius: "12px",
            background: "rgba(255, 255, 255, 0.1)",
          }}
        />
      </div>
      <div className={styles.right}>
        <Skeleton
          variant="rectangular"
          width={60}
          height={24}
          sx={{
            borderRadius: "12px",
            background: "rgba(255, 255, 255, 0.1)",
          }}
        />

        <Skeleton
          variant="rectangular"
          width={60}
          height={24}
          sx={{
            borderRadius: "12px",
            background: "rgba(255, 255, 255, 0.1)",
          }}
        />
      </div>
    </div>
  );
};

export default RaidCardSkeleton;
