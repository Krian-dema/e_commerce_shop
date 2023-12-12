<script setup>
import { ref } from 'vue';
import { vOnClickOutside } from '@vueuse/components';
import { useRouter } from 'vue-router';
import { useForm } from 'vee-validate';
import * as yup from 'yup';

import authApi from '@/api/requests/auth';
import { useUserStore } from '@/stores/user';
import SlIconArrow from '@/components/common/icons/SlIconArrow.vue';
import SlInputField from '@/components/common/SlInputField.vue';
import SlModal from '@/components/common/SlModal.vue';
import SlMessage from '@/components/common/SlMessage.vue';
import { changePasswordErrorMessages } from '@/constants/validationMessages';
import { RouteNames } from '@/enums/routeNames';
import {
  PASSWORD_MIN_LENGTH,
  PASSWORD_MAX_LENGTH,
  PASSWORD_REGEX,
} from '@/constants/validationRules';

defineProps({
  isAdminPanel: Boolean,
});

const emit = defineEmits({ selectOption: null });

const schema = yup.object({
  oldPassword: yup.string().required('Please provide your old password'),
  newPassword: yup
    .string()
    .required('Please provide your new password')
    .min(PASSWORD_MIN_LENGTH, `This field should be more then ${PASSWORD_MIN_LENGTH} characters`)
    .max(PASSWORD_MAX_LENGTH, `This field should be less then ${PASSWORD_MAX_LENGTH} characters`)
    .matches(
      PASSWORD_REGEX,
      'This field should contain at least one uppercase letter and one number or one special character',
    ),
  repeatPassword: yup
    .string()
    .required('Please provide your repeat new password')
    .oneOf([yup.ref('newPassword')], 'Passwords do not match'),
});

const router = useRouter();
const userStore = useUserStore();

const showPanel = ref(false);
const showUpdatePasswordModal = ref(false);
const responseError = ref('');
const loading = ref(false);

const { errors, defineInputBinds, handleSubmit, resetForm } = useForm({
  validationSchema: schema,
});

const oldPassword = defineInputBinds('oldPassword');
const newPassword = defineInputBinds('newPassword');
const repeatPassword = defineInputBinds('repeatPassword');

const handleSelectOption = () => {
  emit('selectOption', true);
};

const togglePanel = () => {
  showPanel.value = !showPanel.value;
};

const closePanel = () => {
  handleSelectOption();
  showPanel.value = false;
};

const toggleUpdatePasswordModal = () => {
  resetForm();
  showUpdatePasswordModal.value = !showUpdatePasswordModal.value;
  closePanel();
};

const closeUpdatePasswordModal = () => {
  responseError.value = '';
  showUpdatePasswordModal.value = false;
};

const logout = async () => {
  closePanel();
  router.push({ name: RouteNames.signIn });
};

const updatePassword = handleSubmit(async (values) => {
  loading.value = true;

  const response = await authApi.changePassword(
    {
      currentPassword: values.oldPassword,
      password: values.newPassword,
      passwordConfirmation: values.repeatPassword,
    },
    userStore.token,
  );

  if (!response?.jwt) {
    const responseErrorMessage = response?.data?.error?.message;
    changePasswordErrorMessages.forEach((error) => {
      if (responseErrorMessage === error.strapiMessage) {
        responseError.value = error.skillslineMessage;
        return;
      }
    });

    if (!responseError.value) {
      responseError.value = responseErrorMessage;
    }
    loading.value = false;

    return;
  }

  responseError.value = '';
  closeUpdatePasswordModal();
  loading.value = false;
});
</script>

<template>
  <div
    class="relative text-cyan-800 font-semibold"
    :class="{ 'mx-8': isAdminPanel }"
    v-on-click-outside="closePanel"
  >
    <div class="flex flex-col justify-center items-start gap-1">
      <div class="flex flex-row items-center gap-3 hover:cursor-pointer" @click="togglePanel">
        <span>{{ userStore.displayName }}</span>
        <SlIconArrow class="h-4 w-4 stroke-3" :class="isAdminPanel ? '-rotate-90' : ' rotate-90'" />
      </div>
    </div>

    <ul v-show="showPanel" :class="isAdminPanel ? 'sl-admin-panel' : 'sl-panel'">
      <li class="block py-2 px-4">
        <span class="text-sm w-full overflow-hidden text-ellipsis">{{ userStore.email }}</span>
      </li>
      <hr/>
      <li class="block py-2 px-4 hover:cursor-pointer" @click="toggleUpdatePasswordModal">
        Reset password
      </li>
      <li class="block py-2 px-4 hover:cursor-pointer" @click="logout">Log Out</li>
    </ul>

    <SlModal
      :showModal="showUpdatePasswordModal"
      saveLabel="Reset"
      styleClass="md:max-w-sm"
      @handleCancel="toggleUpdatePasswordModal"
      @handleSave="updatePassword"
    >
      <div class="flex flex-col items-center gap-4">
        <h2 class="text-2xl font-semibold">Update Password</h2>

        <SlMessage v-if="responseError" :label="responseError" class="w-full" />

        <div class="flex flex-col items-center gap-6 w-full">
          <SlInputField
            v-bind="oldPassword"
            type="password"
            inputId="old-password"
            label="Old Password"
            class="w-full"
            :error="errors.oldPassword"
          />
          <SlInputField
            v-bind="newPassword"
            type="password"
            inputId="new-password"
            label="New Password"
            class="w-full"
            :error="errors.newPassword"
          />
          <SlInputField
            v-bind="repeatPassword"
            type="password"
            inputId="repeat-password"
            label="Repeat New Password"
            class="w-full"
            :error="errors.repeatPassword"
          />
        </div>
      </div>
    </SlModal>
  </div>
</template>

<style scoped>
.sl-panel {
  @apply absolute right-0 top-full z-10 mt-4 py-2 w-full min-w-max rounded-md bg-white border shadow focus:outline-none whitespace-nowrap;
}

.sl-admin-panel {
  @apply absolute right-0 bottom-full z-10 mb-4 py-2 w-full min-w-max rounded-md bg-white border shadow focus:outline-none whitespace-nowrap;
}
</style>
