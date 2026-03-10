"use client";

import { SalaryService } from "../../reports.service";
import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { SalaryDto } from "@/domains/reports";

export function useOpenRegistration() {
  return useProtectedSWRMutation<SalaryDto, number>(
    "/open-Registration",
    (token, id) => SalaryService.openRegistration(token, id),
  );
}
