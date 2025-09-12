import { Tab, Tabs } from "@mui/material";

type Props = {
  value: number;
  setValue: React.Dispatch<React.SetStateAction<number>>;
};

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
    >
      <Tab value={0} label="Общая информация" />
      <Tab value={1} label="Участники" />
      <Tab value={2} label="Скриншоты" />
      <Tab value={3} label="Лут" />
    </Tabs>
  );
};

export default RaidTabs;
