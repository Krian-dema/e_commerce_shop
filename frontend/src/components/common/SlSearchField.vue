<script setup>
import { debounce } from 'lodash';

import SlIconSearch from '@/components/common/icons/SlIconSearch.vue';

const handleChangeDebounce = debounce((value) => emit('changeValue', value), 300);

defineProps({
  value: String,
  placeholder: String,
});

const emit = defineEmits({ changeValue: null, blurEffect: null });

const handleChange = (event) => {
  handleChangeDebounce.cancel();
  handleChangeDebounce(event.target.value);
};

const handleBlurEffect = (event) => {
  emit('blurEffect', event);
};
</script>

<template>
  <div class="relative">
    <div class="absolute inset-y-0 left-0 flex items-center pl-3 pointer-events-none">
      <SlIconSearch class="w-5 h-5 text-gray-500" />
    </div>
    <input
      :value="value"
      type="text"
      :placeholder="placeholder"
      class="sl-search-field"
      autocomplete="off"
      @input="handleChange"
      @blur="handleBlurEffect"
    />
  </div>
</template>

<style scoped>
.sl-search-field {
  @apply block border border-primary-500 rounded-lg text-sm h-full w-full pl-10 pr-24 py-4 focus:border-primary-700 focus:ring-primary-700 focus:outline-none;
}
</style>
