"use client";

import React, { useState, useMemo, useCallback } from "react";
import { motion } from "framer-motion";
import styles from "./_styles.module.scss";
import { useGetRaids } from "@/domains/raids/hooks/useGetRaids";
import RaidsFilters from "@/(main-pages)/raids/components/RaidsFilters/RaidsFilters";

import RaidCard from "@/(main-pages)/raids/components/RaidCard/RaidCard";
import { Divider } from "@mui/material";
import Link from "next/link";
import { RaidListQuery } from "@/domains/raids/raids.service";

import { RaidListItem } from "@/domains/raids/types";
import { PaginatedList } from "@/types/paginated.types";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";
import CreateRaidForm from "@/(main-pages)/raids/components/CreateRaidForm/CreateRaidForm";
import RaidPagination from "../RaidPagination/RaidPagination";
import RaidCardSkeleton from "../RaidCardSkeleton/RaidCardSkeleton";
import DetailContainer from "@/components/ui/DetailContainer/DetailContainer";

type Props = {
  prefetchRaids: PaginatedList<RaidListItem> | undefined;
};

const RaidList = ({ prefetchRaids }: Props) => {
  const [filters, setFilters] = useState<Partial<RaidListQuery>>({
    SortBy: 0,
    SortMode: 2,
  });
  const [page, setPage] = useState(0);
  const [pageSize, setPageSize] = useState(10);

  const filtersWithPagination = useMemo(
    () => ({
      ...filters,
      Skip: page * pageSize,
      Take: pageSize,
    }),
    [filters, page, pageSize]
  );

  const { raids, totalCount, isLoading } = useGetRaids(
    prefetchRaids,
    filtersWithPagination
  );

  // Анимационные варианты
  const containerVariants = {
    hidden: { opacity: 0 },
    visible: {
      opacity: 1,
      transition: {
        delayChildren: 0.2,
        staggerChildren: 0.1,
      },
    },
  };

  const itemVariants = {
    hidden: { y: 20, opacity: 0 },
    visible: {
      y: 0,
      opacity: 1,
      transition: {
        duration: 0.5,
        ease: "easeOut" as const,
      },
    },
  };

  // Сбрасываем страницу при изменении фильтров - используем useCallback для стабильной ссылки
  const handleFiltersChange = useCallback(
    (newFilters: Partial<RaidListQuery>) => {
      setFilters(newFilters);
      setPage(0);
    },
    []
  );

  if (!prefetchRaids) {
    return (
      <ErrorLoadData message="Произошла ошибка при загрузке списка рейдов..." />
    );
  }

  return (
    <DetailContainer>
      <motion.div variants={itemVariants}>
        <CreateRaidForm />
      </motion.div>
      <div className={styles.wrapper}>
        <div className={styles.raidsList}>
          <motion.div
            variants={containerVariants}
            initial="hidden"
            animate="visible"
          >
            <motion.div
              className={styles.raids}
              style={{ minHeight: `${pageSize * 80}px` }}
              variants={containerVariants}
            >
              {isLoading
                ? Array.from({ length: pageSize }).map((_, index) => (
                    <motion.div
                      key={`skeleton-${index}`}
                      variants={itemVariants}
                    >
                      <RaidCardSkeleton />
                    </motion.div>
                  ))
                : raids.map((raid: RaidListItem) => (
                    <motion.div
                      key={raid.id}
                      variants={itemVariants}
                      whileHover={{ scale: 1.02 }}
                      transition={{ duration: 0.2 }}
                    >
                      <Link href={`/raids/${raid.id}`} className={styles.link}>
                        <RaidCard raid={raid} />
                      </Link>
                    </motion.div>
                  ))}
            </motion.div>
          </motion.div>
        </div>
        <motion.div variants={itemVariants} transition={{ duration: 0.2 }}>
          <RaidsFilters initial={filters} onApply={handleFiltersChange} />
        </motion.div>
      </div>
      <motion.div variants={itemVariants}>
        <RaidPagination
          totalCount={totalCount}
          page={page}
          pageSize={pageSize}
          onPageChange={setPage}
          onPageSizeChange={setPageSize}
          isLoading={isLoading}
        />
      </motion.div>
    </DetailContainer>
  );
};

export default RaidList;
