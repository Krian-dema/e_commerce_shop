<script setup>
import { ref, computed } from 'vue';
import { capitalize } from 'lodash';

import SlIconEye from '@/components/common/icons/SlIconEye.vue';
import SlIconCloseEye from '@/components/common/icons/SlIconCloseEye.vue';

const PASSWORD_TYPE = 'password';
const TEXT_TYPE = 'text';

const isVisiblePassword = ref(false);

const props = defineProps({
  inputId: {
    type: String,
    required: true,
  },
  label: {
    type: String,
    required: true,
  },
  type: { type: String, default: 'text' },
  defaultValue: String,
  error: String,
});

defineEmits({ changeValue: null });

const currentType = computed(() =>
  props.type === PASSWORD_TYPE ? (isVisiblePassword.value ? TEXT_TYPE : PASSWORD_TYPE) : props.type,
);
const isPasswordField = computed(() => props.type === PASSWORD_TYPE);
const hasError = computed(() => !!props.error);

const togglePasswordVisibility = () => {
  isVisiblePassword.value = !isVisiblePassword.value;
};
</script>

<template>
  <div>
    <label :for="inputId" class="text-xs mb-2">{{ label }}</label>
    <div class="relative">
      <input
        :id="inputId"
        :type="currentType"
        :placeholder="capitalize(label)"
        :class="{
          'sl-error': hasError,
          'sl-password': isPasswordField,
        }"
        :defaultValue="defaultValue"
        class="sl-input"
        autocomplete="off"
        @input="$emit('changeValue', $event.target.value)"
      />
      <div v-if="isPasswordField" class="absolute top-4 right-4 hover:cursor-pointer">
        <template v-if="!isVisiblePassword">
          <SlIconEye class="w-6 h-6" @click="togglePasswordVisibility" />
        </template>
        <template v-else>
          <SlIconCloseEye class="w-6 h-6" @click="togglePasswordVisibility" />
        </template>
      </div>
    </div>
    <div v-if="hasError" class="text-xs text-red-500 mt-1">
      {{ error }}
    </div>
  </div>
</template>

<style scoped>
.sl-input {
  @apply border rounded-lg w-full px-3 py-3.5 border-primary-500 focus:border-primary-700 focus:ring-primary-700 focus:outline-none;
}

.sl-password {
  @apply pr-14;
}

.sl-error {
  @apply border-red-500 focus:text-black focus:border-red-500 focus:ring-red-500;
}
</style>
