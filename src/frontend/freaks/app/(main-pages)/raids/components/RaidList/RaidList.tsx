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

  // Обновляем фильтры с пагинацией - используем useMemo для предотвращения бесконечного цикла
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
    <section className={styles.raidsPage}>
      <div className={styles.wrapper}>
        <motion.div
          variants={containerVariants}
          initial="hidden"
          animate="visible"
        >
          <motion.div variants={itemVariants}>
            <RaidsFilters initial={filters} onApply={handleFiltersChange} />
          </motion.div>

          <motion.div variants={itemVariants}>
            <CreateRaidForm />
          </motion.div>

          <motion.div className={styles.raids} variants={containerVariants}>
            {raids.length === 0 || isLoading
              ? Array.from({ length: pageSize }).map((_, index) => (
                  <motion.div key={`skeleton-${index}`} variants={itemVariants}>
                    <RaidCardSkeleton />
                    <Divider />
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
                    <Divider />
                  </motion.div>
                ))}
          </motion.div>

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
        </motion.div>
      </div>
    </section>
  );
};

export default RaidList;
