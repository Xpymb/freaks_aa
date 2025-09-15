"use client";

import React, { useState, useEffect } from "react";
import {
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Chip,
  Select,
  MenuItem,
  FormControl,
  Button,
  CircularProgress,
  Alert,
} from "@mui/material";
import { Save as SaveIcon } from "@mui/icons-material";
import {
  useAdminUsers,
  useUpdateUserRoles,
} from "@/domains/user/hooks/useAdminUsers";
import { USER_ROLES, ROLE_LABELS, ROLE_COLORS } from "@/domains/user/constants";
import { CustomTypography } from "@/components/ui/CustomTypography";
import ErrorLoadData from "@/components/ui/ErrorLoadData/ErrorLoadData";
import DefaultLoader from "@/components/ui/DefaultLoader/DefaultLoader";
import type { IUser } from "@/types/user.types";
import styles from "./_styles.module.scss";
import { HelpHint } from "@/components/ui/HelpHint/HelpHint";

type UserWithPendingChanges = IUser & {
  pendingRoles?: number[];
  hasChanges?: boolean;
};

const UsersTable = () => {
  const { users, isLoading, errorState, refresh } = useAdminUsers();
  const { trigger: updateUserRoles, isMutating } = useUpdateUserRoles();

  const [usersWithChanges, setUsersWithChanges] = useState<
    UserWithPendingChanges[]
  >([]);

  // Синхронизируем локальное состояние с данными из API
  useEffect(() => {
    if (users.length > 0) {
      setUsersWithChanges((prevUsers) => {
        // Проверяем, нужно ли обновлять состояние
        if (
          prevUsers.length !== users.length ||
          users.some(
            (user, index) =>
              !prevUsers[index] ||
              prevUsers[index].id !== user.id ||
              JSON.stringify(prevUsers[index].roles) !==
                JSON.stringify(user.roles)
          )
        ) {
          return users.map((user) => ({
            ...user,
            pendingRoles: [...user.roles],
            hasChanges: false,
          }));
        }
        return prevUsers;
      });
    }
  }, [users]);

  // Обновляем данные при событии refresh
  useEffect(() => {
    const handleRefresh = () => {
      refresh();
    };

    window.addEventListener("refresh-admin-users", handleRefresh);
    return () =>
      window.removeEventListener("refresh-admin-users", handleRefresh);
  }, [refresh]);

  const handleRoleChange = (userId: string, newRoles: number[]) => {
    setUsersWithChanges((prev) =>
      prev.map((user) =>
        user.id === userId
          ? {
              ...user,
              pendingRoles: newRoles,
              hasChanges:
                JSON.stringify(newRoles.sort()) !==
                JSON.stringify(user.roles.sort()),
            }
          : user
      )
    );
  };

  const handleSaveChanges = async (userId: string) => {
    const user = usersWithChanges.find((u) => u.id === userId);
    if (!user || !user.pendingRoles || !user.hasChanges) return;

    try {
      await updateUserRoles({
        userId,
        userRoles: user.pendingRoles,
      });

      // После успешного обновления сбрасываем флаги изменений
      setUsersWithChanges((prev) =>
        prev.map((u) =>
          u.id === userId
            ? { ...u, roles: user.pendingRoles!, hasChanges: false }
            : u
        )
      );
    } catch (error) {
      console.error("Failed to update user roles:", error);
    }
  };

  const getRoleChips = (roles: number[]) => {
    return roles.map((roleId) => (
      <Chip
        key={roleId}
        label={ROLE_LABELS[roleId] || `Роль ${roleId}`}
        color={ROLE_COLORS[roleId] || "default"}
        size="small"
        className={styles.roleChip}
      />
    ));
  };

  if (isLoading) {
    return <DefaultLoader />;
  }

  if (errorState && errorState.isError) {
    console.error("Admin users loading error:", {
      errorState,
      message: errorState.message,
      status: errorState.status,
      code: errorState.code,
      details: errorState.details,
    });
    return (
      <ErrorLoadData
        message={`Ошибка при загрузке пользователей: ${
          errorState.message ||
          `HTTP ${errorState.status || "неизвестный статус"}` ||
          "Неизвестная ошибка"
        }`}
      />
    );
  }

  if (!users.length) {
    return <Alert severity="info">Пользователи не найдены</Alert>;
  }

  return (
    <div className={styles.usersContainer}>
      <div className={styles.header}>
        <CustomTypography variant="h5" className={styles.title}>
          Управление пользователями
        </CustomTypography>
        <CustomTypography variant="body2" className={styles.subtitle}>
          Всего пользователей: {users.length}
        </CustomTypography>
      </div>

      <TableContainer className={styles.tableContainer}>
        <Table className={styles.table}>
          <TableHead className={styles.tableHead}>
            <TableRow className={styles.headRow}>
              <TableCell className={styles.headCell}>
                Имя пользователя
                <HelpHint title="Имя пользователя в Discord" />
              </TableCell>
              <TableCell className={styles.headCell}>Игровой ник</TableCell>
              <TableCell className={styles.headCell}>Роли</TableCell>
              <TableCell className={styles.headCell}>Действия</TableCell>
            </TableRow>
          </TableHead>
          <TableBody className={styles.tableBody}>
            {usersWithChanges.map((user) => (
              <TableRow key={user.id} className={styles.bodyRow}>
                <TableCell className={styles.bodyCell}>
                  {user.username}
                </TableCell>
                <TableCell className={styles.bodyCell}>
                  {user.gameNickname}
                </TableCell>
                <TableCell className={styles.bodyCell}>
                  <FormControl className={styles.roleSelect}>
                    <Select
                      multiple
                      value={user.pendingRoles || user.roles}
                      onChange={(e) =>
                        handleRoleChange(user.id, e.target.value as number[])
                      }
                      renderValue={(selected) => (
                        <div className={styles.roleChips}>
                          {getRoleChips(selected as number[])}
                        </div>
                      )}
                      className={styles.selectInput}
                    >
                      {Object.entries(USER_ROLES).map(([key, value]) => (
                        <MenuItem key={value} value={value}>
                          {ROLE_LABELS[value]}
                        </MenuItem>
                      ))}
                    </Select>
                  </FormControl>
                </TableCell>
                <TableCell className={styles.bodyCell}>
                  <Button
                    variant="contained"
                    size="small"
                    startIcon={<SaveIcon />}
                    onClick={() => handleSaveChanges(user.id)}
                    disabled={!user.hasChanges || isMutating}
                    color="primary"
                  >
                    {isMutating ? <CircularProgress size={16} /> : "Сохранить"}
                  </Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </div>
  );
};

export default UsersTable;
