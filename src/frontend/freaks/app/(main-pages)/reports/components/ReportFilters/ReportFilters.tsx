import { Box } from "@mui/material";
import { CustomTypography } from "@/components";
import styles from "./_styles.module.scss";

const ReportFilters = () => {
  return (
    <Box className={styles.filtersRow}>
      <Box className={styles.filter}>
        <CustomTypography variant="body1">Filter 1</CustomTypography>
      </Box>
      <Box className={styles.filter}>
        <CustomTypography variant="body1">Filter 2</CustomTypography>
      </Box>
      <Box className={styles.filter}>
        <CustomTypography variant="body1">Filter 3</CustomTypography>
      </Box>
    </Box>
  );
};

export default ReportFilters;
