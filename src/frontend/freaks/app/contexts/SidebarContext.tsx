"use client";

import React, { createContext, useContext, useState, useEffect } from "react";

interface SidebarContextType {
  isExpanded: boolean;
  setIsExpanded: (expanded: boolean) => void;
  isMobile: boolean;
  isCollapsed: boolean;
  setIsCollapsed: (collapsed: boolean) => void;
}

const SidebarContext = createContext<SidebarContextType | undefined>(undefined);

export const useSidebar = () => {
  const context = useContext(SidebarContext);
  if (context === undefined) {
    throw new Error("useSidebar must be used within a SidebarProvider");
  }
  return context;
};

interface SidebarProviderProps {
  children: React.ReactNode;
}

export const SidebarProvider = ({ children }: SidebarProviderProps) => {
  const [isExpanded, setIsExpanded] = useState(false);
  const [isCollapsed, setIsCollapsed] = useState(false);
  const [isMobile, setIsMobile] = useState(false);

  useEffect(() => {
    const checkMobile = () => {
      const mobile = window.innerWidth <= 768;
      setIsMobile(mobile);
      if (mobile) {
        setIsCollapsed(true);
      }
    };

    checkMobile();
    window.addEventListener("resize", checkMobile);
    return () => window.removeEventListener("resize", checkMobile);
  }, []);

  // Добавляем класс к layoutWrapper для управления отступами
  useEffect(() => {
    const layoutWrapper = document.querySelector(".layoutWrapper");

    if (layoutWrapper) {
      if (isExpanded && !isMobile) {
        layoutWrapper.classList.add("sidebarExpanded");
      } else {
        layoutWrapper.classList.remove("sidebarExpanded");
      }
    }

    return () => {
      if (layoutWrapper) {
        layoutWrapper.classList.remove("sidebarExpanded");
      }
    };
  }, [isExpanded, isMobile]);

  const value = {
    isExpanded,
    setIsExpanded,
    isMobile,
    isCollapsed,
    setIsCollapsed,
  };

  return (
    <SidebarContext.Provider value={value}>{children}</SidebarContext.Provider>
  );
};
