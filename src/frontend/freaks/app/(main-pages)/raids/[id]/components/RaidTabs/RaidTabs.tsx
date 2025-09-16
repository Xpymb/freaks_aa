import { Tab, Tabs } from "@mui/material";
import styles from "./_styles.module.scss";
import { motion } from "framer-motion";

type Props = {
  value: number;
  setValue: React.Dispatch<React.SetStateAction<number>>;
};

const tabs = [
  {
    label: "Общая информация",
    value: 0,
  },
  {
    label: "Участники",
    value: 1,
  },
  {
    label: "Скриншоты",
    value: 2,
  },
  {
    label: "Лут",
    value: 3,
  },
];

const RaidTabs = ({ value, setValue }: Props) => {
  const handleChange = (event: React.SyntheticEvent, value: number) => {
    setValue(value);
  };

  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ duration: 0.5, ease: "easeOut" as const }}
    >
      <Tabs
        value={value}
        onChange={handleChange}
        textColor="secondary"
        indicatorColor="secondary"
        className={styles.tabs}
      >
        {tabs.map((tab) => (
          <Tab
            key={tab.value}
            className={styles.tab}
            classes={{
              selected: styles.selectedTab,
            }}
            value={tab.value}
            label={tab.label}
          />
        ))}
      </Tabs>
    </motion.div>
  );
};

export default RaidTabs;
