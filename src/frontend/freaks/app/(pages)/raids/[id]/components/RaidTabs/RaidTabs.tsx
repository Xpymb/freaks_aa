import { Tab, Tabs } from "@mui/material";
import styles from "./_styles.module.scss";

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
    <Tabs
      value={value}
      onChange={handleChange}
      textColor="secondary"
      indicatorColor="secondary"
      className={styles.tabs}
    >
      {tabs.map((tab) => (
        <Tab
          className={styles.tab}
          classes={{
            selected: styles.selectedTab,
          }}
          key={tab.value}
          value={tab.value}
          label={tab.label}
        />
      ))}
    </Tabs>
  );
};

export default RaidTabs;
