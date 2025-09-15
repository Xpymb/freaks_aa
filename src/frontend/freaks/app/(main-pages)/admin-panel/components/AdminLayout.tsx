"use client";

import React, { useState } from "react";
import { Box, Tab, Tabs, Paper } from "@mui/material";
import {
  People as PeopleIcon,
  Settings as SettingsIcon,
} from "@mui/icons-material";
import { CustomTypography } from "@/components/ui/CustomTypography";
import styles from "./_styles.module.scss";

interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`vertical-tabpanel-${index}`}
      aria-labelledby={`vertical-tab-${index}`}
      className={styles.tabPanel}
      {...other}
    >
      {value === index && <Box sx={{ p: 3 }}>{children}</Box>}
    </div>
  );
}

function a11yProps(index: number) {
  return {
    id: `vertical-tab-${index}`,
    "aria-controls": `vertical-tabpanel-${index}`,
  };
}

type Props = {
  children: React.ReactNode;
};

const AdminLayout = ({ children }: Props) => {
  const [activeTab, setActiveTab] = useState(0);

  const handleTabChange = (event: React.SyntheticEvent, newValue: number) => {
    setActiveTab(newValue);
  };

  return (
    <div className={styles.adminContainer}>
      <div className={styles.header}>
        <CustomTypography variant="h1" className={styles.title}>
          Панель администрирования
        </CustomTypography>
        <CustomTypography variant="subtitle1" className={styles.subtitle}>
          Управление пользователями и настройками системы
        </CustomTypography>
      </div>

      <div className={styles.content}>
        <Paper className={styles.tabsContainer}>
          <Tabs
            orientation="vertical"
            variant="scrollable"
            value={activeTab}
            onChange={handleTabChange}
            className={styles.tabs}
            TabIndicatorProps={{
              className: styles.tabIndicator,
            }}
          >
            <Tab
              icon={<PeopleIcon />}
              label="Пользователи"
              className={styles.tab}
              classes={{
                selected: styles.selectedTab,
              }}
              {...a11yProps(0)}
            />
            <Tab
              icon={<SettingsIcon />}
              label="Настройки"
              className={styles.tab}
              classes={{
                selected: styles.selectedTab,
              }}
              {...a11yProps(1)}
              disabled
            />
          </Tabs>
        </Paper>

        <Paper className={styles.contentPanel}>
          <TabPanel value={activeTab} index={0}>
            {children}
          </TabPanel>
          <TabPanel value={activeTab} index={1}>
            <CustomTypography variant="h6">
              Настройки системы (в разработке)
            </CustomTypography>
          </TabPanel>
        </Paper>
      </div>
    </div>
  );
};

export default AdminLayout;
