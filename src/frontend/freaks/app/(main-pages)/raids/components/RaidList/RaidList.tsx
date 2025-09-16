"use client";

import React, { useState, useMemo, useCallback } from "react";
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
  const [pageSize, setPageSize] = useState(5);

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
    <div className={styles.wrapper}>
      <RaidsFilters initial={filters} onApply={handleFiltersChange} />

      <CreateRaidForm />

      <div className={styles.raids}>
        {raids.length === 0 || isLoading
          ? Array.from({ length: pageSize }).map((_, index) => (
              <React.Fragment key={`skeleton-${index}`}>
                <RaidCardSkeleton />
                <Divider />
              </React.Fragment>
            ))
          : raids.map((raid) => (
              <React.Fragment key={raid.id}>
                <Link href={`/raids/${raid.id}`} className={styles.link}>
                  <RaidCard raid={raid} />
                </Link>
                <Divider />
              </React.Fragment>
            ))}
      </div>

      <RaidPagination
        totalCount={totalCount}
        page={page}
        pageSize={pageSize}
        onPageChange={setPage}
        onPageSizeChange={setPageSize}
        isLoading={isLoading}
      />
    </div>
  );
};

export default RaidList;
