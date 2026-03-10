"use client";

import { SalaryService } from "../../reports.service";
import { useProtectedSWRMutation } from "@/shared/api/useProtectedSWRMutation";
import { SalaryDto } from "@/domains/reports";

export function useCloseRegistration() {
  return useProtectedSWRMutation<SalaryDto, number>(
    "/close-Registration",
    (token, id) => SalaryService.closeRegistration(token, id),
  );
}
