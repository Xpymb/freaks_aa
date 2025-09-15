"use client";

import React, { useMemo } from "react";
import { usePathname } from "next/navigation";
import Link from "next/link";
import { useSidebar } from "@/contexts/SidebarContext";
import { useHasRole } from "@/domains/auth/hooks/useHasRole";
import {
  AccessTimeOutlined,
  EmailOutlined,
  ShoppingCartOutlined,
  HomeOutlined,
  PeopleOutlined,
  MenuOutlined,
  ChevronRightOutlined,
  ChevronLeftOutlined,
  AdminPanelSettings,
} from "@mui/icons-material";
import { Tooltip, IconButton } from "@mui/material";
import clsx from "clsx";
import styles from "./_styles.module.scss";

type NavigationItem = {
  id: string;
  label: string;
  icon: React.ReactNode;
  href: string;
  disabled?: boolean;
};

const navigationItems: NavigationItem[] = [
  {
    id: "overview",
    label: "Обзор",
    icon: <HomeOutlined />,
    href: "/overview",
  },
  {
    id: "raids",
    label: "Рейды",
    icon: <AccessTimeOutlined />,
    href: "/raids",
  },
  {
    id: "mail",
    label: "Сообщения",
    icon: <EmailOutlined />,
    href: "/messages",
    disabled: true, // пока не реализовано
  },
  {
    id: "shop",
    label: "Магазин",
    icon: <ShoppingCartOutlined />,
    href: "/shop",
    disabled: true, // пока не реализовано
  },
  {
    id: "home",
    label: "Главная",
    icon: <HomeOutlined />,
    href: "/",
    disabled: true, // пока не реализовано
  },
  {
    id: "guild",
    label: "Гильдия",
    icon: <PeopleOutlined />,
    href: "/guild",
    disabled: true, // пока не реализовано
  },
];

const Sidebar = () => {
  const pathname = usePathname();
  const { isExpanded, setIsExpanded, isMobile, isCollapsed, setIsCollapsed } =
    useSidebar();

  const isActiveRoute = (href: string) => {
    if (href === "/") {
      return pathname === "/";
    }
    return pathname.startsWith(href);
  };

  const toggleSidebar = () => {
    setIsCollapsed(!isCollapsed);
  };

  const toggleExpanded = () => {
    setIsExpanded(!isExpanded);
  };

  return (
    <>
      {/* Mobile overlay */}
      {isMobile && !isCollapsed && (
        <div className={styles.overlay} onClick={() => setIsCollapsed(true)} />
      )}

      <aside
        className={clsx(styles.sidebar, {
          [styles.collapsed]: isCollapsed,
          [styles.mobile]: isMobile,
          [styles.expanded]: isExpanded && !isMobile,
        })}
      >
        {/* Toggle button for mobile */}
        {isMobile && (
          <div className={styles.toggleButton}>
            <IconButton
              onClick={toggleSidebar}
              className={styles.menuButton}
              size="small"
            >
              <MenuOutlined />
            </IconButton>
          </div>
        )}

        <nav className={styles.navigation}>
          {navigationItems.map((item) => {
            const isActive = isActiveRoute(item.href);
            const isDisabled = item.disabled;

            const buttonContent = (
              <div
                className={clsx(styles.navItem, {
                  [styles.active]: isActive && !isDisabled,
                  [styles.disabled]: isDisabled,
                  [styles.expandedItem]: isExpanded && !isMobile,
                })}
              >
                <IconButton
                  className={styles.iconButton}
                  disabled={isDisabled}
                  disableRipple={isDisabled}
                >
                  {item.icon}
                </IconButton>
                {isExpanded && !isMobile && (
                  <span className={styles.navLabel}>{item.label}</span>
                )}
              </div>
            );

            if (isDisabled) {
              return (
                <Tooltip
                  key={item.id}
                  title={`${item.label} (скоро)`}
                  placement="right"
                  disableInteractive={isExpanded && !isMobile}
                >
                  <div>{buttonContent}</div>
                </Tooltip>
              );
            }

            return (
              <Tooltip
                key={item.id}
                title={item.label}
                placement="right"
                disableInteractive={isExpanded && !isMobile}
              >
                <Link href={item.href} className={styles.navLink}>
                  {buttonContent}
                </Link>
              </Tooltip>
            );
          })}
        </nav>

        {/* Кнопка раздвижения для десктопа */}
        {!isMobile && (
          <div className={styles.expandToggle}>
            <IconButton
              onClick={toggleExpanded}
              className={styles.expandButton}
              size="small"
            >
              {isExpanded ? <ChevronLeftOutlined /> : <ChevronRightOutlined />}
            </IconButton>
          </div>
        )}
      </aside>
    </>
  );
};

export default Sidebar;
