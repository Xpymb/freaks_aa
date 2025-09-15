"use client";

import React from "react";
import { Button } from "@mui/material";
import {
  Block as BlockIcon,
  Home as HomeIcon,
  ArrowBack as ArrowBackIcon,
} from "@mui/icons-material";
import { useRouter } from "next/navigation";
import { CustomTypography } from "@/components/ui/CustomTypography";
import { CustomContainer } from "@/components/ui/CustomContainer";
import styles from "./_styles.module.scss";

const ForbiddenPage = () => {
  const router = useRouter();

  return (
    <div className={styles.container}>
      <CustomContainer maxWidth="md">
        <div className={styles.wrapper}>
          <div className={styles.content}>
            <div className={styles.iconWrapper}>
              <BlockIcon className={styles.icon} />
            </div>

            <div className={styles.textContent}>
              <CustomTypography variant="h1" className={styles.errorCode}>
                403
              </CustomTypography>

              <CustomTypography variant="h4" className={styles.title}>
                Доступ запрещен
              </CustomTypography>

              <CustomTypography variant="body1" className={styles.description}>
                У вас нет необходимых прав доступа для просмотра этой страницы.
                <br />
                Обратитесь к администратору для получения доступа.
              </CustomTypography>

              <div className={styles.actions}>
                <Button
                  variant="contained"
                  startIcon={<HomeIcon />}
                  onClick={() => router.push("/")}
                  className={styles.primaryButton}
                >
                  На главную
                </Button>

                <Button
                  variant="outlined"
                  startIcon={<ArrowBackIcon />}
                  onClick={() => router.back()}
                  className={styles.secondaryButton}
                >
                  Назад
                </Button>
              </div>
            </div>
          </div>
        </div>
      </CustomContainer>
    </div>
  );
};

export default ForbiddenPage;
