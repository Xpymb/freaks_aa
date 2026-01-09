"use client";

import React from "react";
import { usePathname } from "next/navigation";
import Link from "next/link";
import Image from "next/image";
import { useSidebar } from "@/contexts/SidebarContext";

import {
  ChevronLeftOutlined,
  ChevronRightOutlined,
  EmailOutlined,
  HomeOutlined,
  MenuOutlined,
  PeopleOutlined,
  ShoppingCartOutlined,
} from "@mui/icons-material";
import { IconButton, Tooltip } from "@mui/material";
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
    label: "Главная",
    icon: (
      <Image
        src={"/icons/home.svg"}
        alt="overviewIcon"
        width={16}
        height={16}
      />
    ),
    href: "/overview",
  },
  {
    id: "raids",
    label: "Рейды",
    icon: (
      <Image
        src={"/icons/raids.svg"}
        alt="overviewIcon"
        width={16}
        height={16}
      />
    ),
    href: "/raids",
  },
  {
    id: "reports",
    label: "Отчеты",
    icon: (
      <Image
        src={"/icons/reports.svg"}
        alt="overviewIcon"
        width={16}
        height={16}
      />
    ),
    href: "/reports",
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

            // Если сайдбар развернут - тултип не нужен
            const shouldShowTooltip = !isExpanded || isMobile;

            if (isDisabled) {
              return shouldShowTooltip ? (
                <Tooltip
                  key={item.id}
                  title={`${item.label} (скоро)`}
                  placement="right"
                >
                  <div>{buttonContent}</div>
                </Tooltip>
              ) : (
                <div key={item.id}>{buttonContent}</div>
              );
            }

            return shouldShowTooltip ? (
              <Tooltip key={item.id} title={item.label} placement="right">
                <Link href={item.href} className={styles.navLink}>
                  {buttonContent}
                </Link>
              </Tooltip>
            ) : (
              <Link key={item.id} href={item.href} className={styles.navLink}>
                {buttonContent}
              </Link>
            );
          })}
        </nav>

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
